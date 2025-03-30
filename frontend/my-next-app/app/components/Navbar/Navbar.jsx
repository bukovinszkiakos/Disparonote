"use client";
import React from "react";
import Link from "next/link";
import { useAuth } from "../../context/AuthContext";
import TokenTimer from "../TokenTimer/TokenTimer";
import { Player } from "@lottiefiles/react-lottie-player";
import "./Navbar.css";

const Navbar = () => {
  const { user, logout } = useAuth();

  return (
    <header className="heading">
      <nav className="navbar">
        <Link href="/" className="nav-link">
          <button className="nav-button">
            Home
            <Player
              src="/animations/home-animation.json"
              className="animation"
              autoplay
              loop
            />
          </button>
        </Link>

        {!user ? (
          <>
            <Link href="/login" className="nav-link">
              <button className="nav-button">
                Login
                <Player
                  src="/animations/login-animation.json"
                  className="animation"
                  autoplay
                  loop
                />
              </button>
            </Link>
            <Link href="/register" className="nav-link">
              <button className="nav-button">
                Register
                <Player
                  src="/animations/register-animation.json"
                  className="animation"
                  autoplay
                  loop
                />
              </button>
            </Link>
          </>
        ) : (
          <>
            <Link href="/create-note" className="nav-link">
              <button className="nav-button">
                Create Note
                <Player
                  src="/animations/logo-animation.json"
                  className="animation"
                  autoplay
                  loop
                />
              </button>
            </Link>

            <div className="nav-actions">
              <button onClick={logout} className="logout-button">
                Logout
                <Player
                  src="/animations/logout-animation.json"
                  className="animation"
                  autoplay
                  loop
                />
              </button>
            </div>
            <TokenTimer expirationUnix={user.expirationUnix} />
          </>
        )}
      </nav>
    </header>
  );
};

export default Navbar;
