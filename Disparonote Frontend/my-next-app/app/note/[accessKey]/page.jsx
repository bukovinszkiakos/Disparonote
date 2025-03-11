"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { apiGet } from "../../../utils/api";
import {
  base64ToArrayBuffer,
  importKeyFromBase64,
  decryptText,
} from "../../../utils/crypto";
import "./noteview.css";

export default function NoteView() {
  const params = useParams();
  const { accessKey } = params;
  const [decryptedContent, setDecryptedContent] = useState(null);
  const [error, setError] = useState(null);
  const router = useRouter();

  useEffect(() => {
    if (!accessKey) return; 

    const fetchAndDecryptNote = async () => {
      try {
        
        const response = await apiGet(`/notes/${accessKey}`);
        const encryptedBase64 = response.content;
        
        const encryptedBuffer = base64ToArrayBuffer(encryptedBase64);

        
        const fragment = window.location.hash;
        if (!fragment) {
          throw new Error("Missing decryption key in URL fragment.");
        }
       
        const clientSecretKey = fragment.substring(1);

        
        const key = await importKeyFromBase64(clientSecretKey);

        
        const decrypted = await decryptText(key, new Uint8Array(encryptedBuffer));
        setDecryptedContent(decrypted); 
      } catch (err) {
        console.error("❌ Error during decryption:", err);
        setError("The note was not found, or it has already been viewed, or decryption failed.");
      }
    };

    fetchAndDecryptNote();
  }, [accessKey]);

  return (
    <div className="note-view-container">
      <h2>Note Content</h2>
      {error && <p className="note-error">{error}</p>}
      <div className="note-content-box">
        {decryptedContent ? <p className="note-content">{decryptedContent}</p> : <p>Loading...</p>}
      </div>
      <button className="note-btn" onClick={() => router.push("/")}>Back to homepage</button>
    </div>
  );
}
