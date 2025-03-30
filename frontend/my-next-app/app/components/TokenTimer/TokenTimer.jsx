"use client";
import { useEffect, useState, useRef } from "react";
import { useRouter } from "next/navigation";
import { useAuth } from "../../context/AuthContext";
import { FaClock } from "react-icons/fa";
import "./TokenTimer.css";

export default function TokenTimer({ expirationUnix }) {
  const [timeLeft, setTimeLeft] = useState(null);
  const [expirationDate, setExpirationDate] = useState(null);
  const router = useRouter();
  const intervalRef = useRef(null);
  const { logout } = useAuth();

  useEffect(() => {
    if (!expirationUnix) {
      setTimeLeft(null);
      setExpirationDate(null);
      return;
    }

    const expirationTime = expirationUnix * 1000;
    setExpirationDate(new Date(expirationTime));

    const checkToken = () => {
      const now = Date.now();
      const diff = expirationTime - now;

      if (diff <= 0) {
        clearInterval(intervalRef.current);
        logout();
      } else {
        setTimeLeft(diff);
      }
    };

    checkToken();
    intervalRef.current = setInterval(checkToken, 1000);

    return () => clearInterval(intervalRef.current);
  }, [expirationUnix, logout]);

  if (timeLeft === null || !expirationDate) return null;

  const formatTimeLeft = (ms) => {
    const totalSeconds = Math.floor(ms / 1000);
    const days = Math.floor(totalSeconds / 86400);
    const hours = Math.floor((totalSeconds % 86400) / 3600);
    const minutes = Math.floor((totalSeconds % 3600) / 60);
    const seconds = totalSeconds % 60;
  
    const parts = [];
    if (days > 0) parts.push(`${days} day${days !== 1 ? "s" : ""}`);
    if (hours > 0) parts.push(`${hours} hour${hours !== 1 ? "s" : ""}`);
    if (minutes > 0) parts.push(`${minutes} minute${minutes !== 1 ? "s" : ""}`);
    if (days === 0 && hours === 0) {
      // Only show seconds if we're under an hour
      parts.push(`${seconds} second${seconds !== 1 ? "s" : ""}`);
    }
  
    return parts.join(", ");
  };
  

  const formatExpirationDate = (date) => {
    return date.toLocaleString("en-GB", {
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  return (
    <div className="token-timer-container">
  <FaClock className="token-icon" />
  <div>
    <div>
      Session expires in <strong>{formatTimeLeft(timeLeft)}</strong>
    </div>
    <div style={{ fontSize: "12px", color: "#ccc" }}>
      ({formatExpirationDate(expirationDate)})
    </div>
  </div>
</div>

  );
}
