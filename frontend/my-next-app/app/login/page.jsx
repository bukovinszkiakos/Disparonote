"use client";
import { useState, useEffect } from "react";
import { useAuth } from "../context/AuthContext";
import "./login.css";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);
  const { login } = useAuth();
  const [rememberMe, setRememberMe] = useState(false);

  useEffect(() => {
    const script = document.createElement("script");
    script.type = "module";
    script.src =
      "https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.esm.js";
    document.body.appendChild(script);
  }, []);

  const handleLogin = async (e) => {
    e.preventDefault();
    setError(null);

    try {
      await login(email, password, rememberMe);
    } catch (err) {
      setError("Incorrect email or password");
      console.error(err);
    }
  };

  return (
    <div className="wrapper">
      <div className="form-box login">
        <h2>Login</h2>
        {error && <p style={{ color: "red" }}>{error}</p>}
        <form action="#" onSubmit={handleLogin}>
          <div className="login-input-box">
            <span className="icon">
              <ion-icon name="mail"></ion-icon>
            </span>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
            <label>Email</label>
          </div>

          <div className="login-input-box">
            <span className="icon">
              <ion-icon name="lock-closed"></ion-icon>
            </span>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
            <label>Password</label>
          </div>

          <div className="remember-forgot">
            <label>
              <input
                type="checkbox"
                checked={rememberMe}
                onChange={(e) => setRememberMe(e.target.checked)}
              />
              Remember me
            </label>
          </div>

          <button type="submit" className="btn">
            Login
          </button>

          <div className="login-register">
            <p>
              Don't have an account?{" "}
              <a href="/register" className="register-link">
                Register
              </a>
            </p>
          </div>
        </form>
      </div>
    </div>
  );
}
