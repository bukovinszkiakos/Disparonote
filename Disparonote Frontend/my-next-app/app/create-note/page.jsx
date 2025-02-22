"use client";
import { useState } from "react";
import { apiPost } from "../../utils/api";
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
      const data = {
        Content: content,
        ExpiresAt: expiresAt ? new Date(expiresAt) : null,
      };

      console.log("🔄 API call in progress:", data);
      const response = await apiPost("/notes", data);

      console.log("✅ API response:", response);

      if (!response || !response.accessLink) {
        throw new Error("Invalid API response, missing accessLink.");
      }

      const fullLink = `http://localhost:3000/note/${response.accessLink.split("/").pop()}`;
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
          <p><b>📌 Shareable Link:</b></p>
          <input
            type="text"
            value={accessLink}
            readOnly
            style={{ width: "300px" }}
          />
          <button
            onClick={() => navigator.clipboard.writeText(accessLink)}
          >
            Copy
          </button>
        </div>
      )}
    </div>
  );
}
