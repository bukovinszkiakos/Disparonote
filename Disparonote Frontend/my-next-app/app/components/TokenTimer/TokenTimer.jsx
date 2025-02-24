"use client";
import { useEffect, useState, useRef } from "react";
import { useRouter } from "next/navigation";
import { useAuth } from "../../context/AuthContext";
import { FaClock } from "react-icons/fa";
import { apiPost } from "@/utils/api";
import "./TokenTimer.css";

export default function TokenTimer({ expirationUnix }) {
  const [timeLeft, setTimeLeft] = useState(null);
  const router = useRouter();
  const intervalRef = useRef(null);
  const { logout } = useAuth();

  useEffect(() => {
    if (!expirationUnix) {
      setTimeLeft(null);
      return;
    }

    const expirationTime = expirationUnix * 1000;

    const checkToken = async () => {
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
  }, [expirationUnix, router, logout]);

  if (timeLeft === null) return null;

  return (
    <div className="token-timer-container">
      <FaClock className="token-icon" />
      <span>Session expires in: {Math.floor(timeLeft / 1000)} sec</span>
    </div>
  );
}
