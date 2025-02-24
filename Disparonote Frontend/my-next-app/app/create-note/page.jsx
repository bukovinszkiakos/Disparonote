"use client";
import { useState } from "react";
import { apiPost } from "../../utils/api";
import {
  generateEncryptionKey,
  encryptText,
  exportKey,
  arrayBufferToBase64,
} from "../../utils/crypto";
import "./createnote.css";

export default function CreateNote() {
  const [content, setContent] = useState("");
  const [expiresAt, setExpiresAt] = useState("");
  const [accessLink, setAccessLink] = useState(null);
  const [error, setError] = useState(null);

  const handleCreateNote = async (e) => {
    e.preventDefault();
    setError(null);
    setAccessLink(null);

    try {
      const key = await generateEncryptionKey();
      const encryptedBuffer = await encryptText(key, content);
      const encryptedContent = arrayBufferToBase64(encryptedBuffer);
      const exportedKey = await exportKey(key);

      const response = await apiPost("/notes", {
        Content: encryptedContent,
        ExpiresAt: expiresAt ? new Date(expiresAt) : null,
      });

      const noteId = response.accessLink.split("/").pop();
      const fullLink = `${window.location.origin}/note/${noteId}#${exportedKey}`;
      setAccessLink(fullLink);
    } catch (err) {
      console.error("❌ Error:", err);
      setError("Failed to create the note.");
    }
  };

  return (
    <div className="note-wrapper">
      <div className="note-form-box">
        <h2>Create Note</h2>
        {error && <p style={{ color: "red" }}>{error}</p>}

        <form onSubmit={handleCreateNote}>
          <div className="note-input-box">
            <span className="note-icon">
              <ion-icon name="document-text"></ion-icon>
            </span>
            <textarea
              placeholder="Write the content of the note here..."
              value={content}
              onChange={(e) => setContent(e.target.value)}
              required
              rows={5}
            />
          </div>

          <div className="note-input-box">
            <span className="note-icon">
              <ion-icon name="time"></ion-icon>
            </span>
            <input
              type="datetime-local"
              value={expiresAt}
              onChange={(e) => setExpiresAt(e.target.value)}
            />
          </div>

          <button type="submit" className="note-btn">
            Create Note
          </button>
        </form>

        {accessLink && (
          <div className="note-access-link-container">
            <p>
              <b>📌 Shareable Link:</b>
            </p>
            <input type="text" value={accessLink} readOnly />
            <button onClick={() => navigator.clipboard.writeText(accessLink)}>
              Copy
            </button>
          </div>
        )}
      </div>
    </div>
  );
}