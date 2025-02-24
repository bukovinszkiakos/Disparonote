import React from "react";
import Link from "next/link";
import "./Footer.css";

const Footer = () => {
  return (
    <footer className="footer">
      <ul>
        <li><Link href="/about" className="footer-link">About</Link></li>
        <li><Link href="/privacy" className="footer-link">Privacy Policy</Link></li>
        <li><Link href="/tnc" className="footer-link">Terms & Conditions</Link></li>
        <li><Link href="/learn-more" className="footer-link">Other Information</Link></li>
      </ul>
    </footer>
  );
};

export default Footer;
