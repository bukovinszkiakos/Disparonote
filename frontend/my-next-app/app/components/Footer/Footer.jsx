"use client";
import React, { useEffect, useState } from "react";
import "./Footer.css";

const Footer = () => {
  const [year, setYear] = useState("");

  useEffect(() => {
    setYear(new Date().getFullYear());
  }, []);

  return (
    <footer className="footer">
      <p>© {year} Disparonote. All rights reserved.</p>
      <p>Built with ❤️ using Next.js & .NET</p>
    </footer>
  );
};

export default Footer;
