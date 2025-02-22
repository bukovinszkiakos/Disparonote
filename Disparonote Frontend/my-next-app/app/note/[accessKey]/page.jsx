"use client";
import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { apiGet } from "../../../utils/api";  
import "./noteview.css";

export default function NoteView() {
  const params = useParams();
  const { accessKey } = params;
  const [content, setContent] = useState(null);
  const [error, setError] = useState(null);
  const router = useRouter();

  useEffect(() => {
    if (!accessKey) return;

    const fetchNote = async () => {
      try {
        console.log(`🔍 Request: /notes/${accessKey}`);
        const response = await apiGet(`/notes/${accessKey}`);
        console.log("✅ API response:", response);
        setContent(response.content);
      } catch (err) {
        console.error("❌ Error:", err);
        setError("The note was not found, or it has already been viewed.");
      }
    };

    fetchNote();
  }, [accessKey]);

  return (
    <div className="note-view-container">
      <h2>Note Content</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      {content ? <p>{content}</p> : <p>Loading...</p>}
      <button onClick={() => router.push("/")}>Back to homepage</button>
    </div>
  );
}
