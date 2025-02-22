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
    <div className="create-note-container">
      <h2>Create Note</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <form onSubmit={handleCreateNote}>
        <div>
          <label htmlFor="content">Note Content:</label>
          <textarea
            id="content"
            placeholder="Write the content of the note here..."
            value={content}
            onChange={(e) => setContent(e.target.value)}
            required
            rows={5}
            cols={40}
          />
        </div>
        <div>
          <label htmlFor="expiresAt">Expiration time (optional):</label>
          <input
            type="datetime-local"
            id="expiresAt"
            value={expiresAt}
            onChange={(e) => setExpiresAt(e.target.value)}
          />
        </div>
        <button type="submit">Create Note</button>
      </form>

      {accessLink && (
        <div>
          <p>
            <b>📌 Shareable Link:</b>
          </p>
          <input type="text" value={accessLink} readOnly style={{ width: "300px" }} />
          <button onClick={() => navigator.clipboard.writeText(accessLink)}>
            Copy
          </button>
        </div>
      )}
    </div>
  );
}
