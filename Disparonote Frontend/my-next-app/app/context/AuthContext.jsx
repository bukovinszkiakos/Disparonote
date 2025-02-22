"use client";
import React, { createContext, useContext, useState, useEffect } from "react";
import { apiGet, apiPost } from "@/utils/api";
import { useRouter } from "next/navigation";

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const router = useRouter();

  const fetchUser = async () => {
    try {
      const res = await apiGet("/Auth/Me");
      setUser(res);
    } catch (err) {
      setUser(null);
    }
  };

  useEffect(() => {
    fetchUser();
  }, []);

  const login = async (email, password) => {
    try {
      await apiPost("/Auth/Login", { Email: email, Password: password });
      await fetchUser();
      router.push("/create-note");
    } catch (err) {
      throw err;
    }
  };

  const logout = async () => {
    try {
      await apiPost("/Auth/Logout", {});
      setUser(null);
      router.push("/login");
    } catch (err) {
      throw err;
    }
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, fetchUser }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
