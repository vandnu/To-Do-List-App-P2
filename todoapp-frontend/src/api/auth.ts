import axios from "axios";

const API_URL = "https://localhost:7100/api/Auth";

// Login
export const login = async (username: string, password: string) => {
  const response = await axios.post(`${API_URL}/login`, { username, password });
  return response.data.token; // Forventer { token: "..." }
};

// Registrering
export const register = async (username: string, password: string, email: string) => {
  const response = await axios.post(`${API_URL}/register`, { username, password, email });
  return response.data; // Forventer "User created successfully"
};