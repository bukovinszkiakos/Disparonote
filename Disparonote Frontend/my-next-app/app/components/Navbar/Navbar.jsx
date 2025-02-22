"use client";
import React from "react";
import Link from "next/link";
import { useAuth } from "../../context/AuthContext";
import TokenTimer from "../TokenTimer/TokenTimer";
import "./Navbar.css";

const Navbar = () => {
  const { user, logout } = useAuth();

  return (
    <header className="heading">
      <nav className="navbar">
        <Link href="/">
          <button className="btn btn-primary">Home</button>
        </Link>
        <Link href="/create-note">
          <button className="btn btn-primary">CreateNote</button>
        </Link>
        {!user ? (
          <>
            <Link href="/login">
              <button className="btn btn-primary">Login</button>
            </Link>
            <Link href="/register">
              <button className="btn btn-primary">Register</button>
            </Link>
          </>
        ) : (
          <>
            <button onClick={logout} className="btn btn-primary">
              Logout
            </button>
            <TokenTimer expirationUnix={user.expirationUnix} />
          </>
        )}
      </nav>
    </header>
  );
};

export default Navbar;
