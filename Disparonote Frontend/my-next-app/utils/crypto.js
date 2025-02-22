export async function generateEncryptionKey() {
    return crypto.subtle.generateKey(
      { name: "AES-GCM", length: 256 },
      true,
      ["encrypt", "decrypt"]
    );
  }
  
  export async function encryptText(key, text) {
    const encoder = new TextEncoder();
    const data = encoder.encode(text);
    const iv = crypto.getRandomValues(new Uint8Array(12)); 
    const encrypted = await crypto.subtle.encrypt(
      { name: "AES-GCM", iv },
      key,
      data
    );

    const combined = new Uint8Array(iv.byteLength + encrypted.byteLength);
    combined.set(iv, 0);
    combined.set(new Uint8Array(encrypted), iv.byteLength);
    return combined;
  }
  
  export async function exportKey(key) {
    const raw = await crypto.subtle.exportKey("raw", key);
    return btoa(String.fromCharCode(...new Uint8Array(raw)));
  }
  
  export function arrayBufferToBase64(buffer) {
    return btoa(String.fromCharCode(...new Uint8Array(buffer)));
  }
  
  export function base64ToArrayBuffer(base64) {
    const binary = atob(base64);
    const len = binary.length;
    const bytes = new Uint8Array(len);
    for (let i = 0; i < len; i++) {
      bytes[i] = binary.charCodeAt(i);
    }
    return bytes.buffer;
  }

export async function importKeyFromBase64(base64Key) {
    const raw = Uint8Array.from(atob(base64Key), (c) => c.charCodeAt(0));
    return crypto.subtle.importKey(
      "raw",
      raw,
      { name: "AES-GCM" },
      true,
      ["decrypt"]
    );
  }
  
  export async function decryptText(key, encryptedBuffer) {
    const iv = encryptedBuffer.slice(0, 12);
    const ciphertext = encryptedBuffer.slice(12);
    const decrypted = await crypto.subtle.decrypt(
      { name: "AES-GCM", iv },
      key,
      ciphertext
    );
    return new TextDecoder().decode(decrypted);
  }
  