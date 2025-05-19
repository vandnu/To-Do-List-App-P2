import { create } from "zustand";

interface AuthState {
  token: string | null;
  role: string | null;
  login: (token: string, role: string) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  token: null,
  role: null,
  login: (token: string, role: string) => set({ token, role }),
  logout: () => set({ token: null, role: null }),
}));