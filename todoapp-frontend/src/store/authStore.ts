import { create } from "zustand";

interface AuthState {
  token: string | null;
  role: string | null;
  userId: number | null;
  login: (token: string, role: string) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  token: null,
  role: null,
  userId: null,
  login: (token, role) => {
    // Dekoder JWT-token for at hente userId
    const payload = token.split(".")[1];
    const decoded = JSON.parse(atob(payload));
    const userId = parseInt(decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]);
    
    set({ token, role, userId });
    localStorage.setItem("token", token);
    localStorage.setItem("role", role);
  },
  logout: () => {
    set({ token: null, role: null, userId: null });
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    localStorage.removeItem("userId");
  },
}));