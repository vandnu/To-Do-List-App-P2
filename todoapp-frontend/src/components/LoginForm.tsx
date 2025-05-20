import React, { useState } from "react";
import { login } from "../api/auth";
import { useAuthStore } from "../store/authStore";
import { useNavigate } from "react-router-dom";

const LoginForm: React.FC = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState("");
  const { token, login: setAuth, logout } = useAuthStore();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  if (token) {
    return (
      <div className="max-w-md mx-auto mt-10 p-6 bg-white rounded-lg shadow-md">
        <p className="text-center text-gray-700">Allerede logget ind. Log ud først.</p>
        <button
          onClick={handleLogout}
          className="w-full mt-4 bg-red-500 text-white p-2 rounded hover:bg-red-600"
        >
          Log ud
        </button>
      </div>
    );
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const token = await login(username, password);
      const role = username.toLowerCase() === "admin" ? "Admin" : "User";
      setAuth(token, role);
      navigate("/tasks");
    } catch (err) {
      setError("Ugyldig brugernavn eller adgangskode");
    }
  };

  return (
    <div className="max-w-md mx-auto mt-10 p-6 bg-white rounded-lg shadow-md">
      <h2 className="text-2xl font-bold mb-4 text-gray-800">Log ind</h2>
      {error && <p className="text-red-500 mb-4">{error}</p>}
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block mb-1 text-gray-700">Brugernavn:</label>
          <input
            className="w-full p-2 border rounded"
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>
        <div>
          <label className="block mb-1 text-gray-700">Adgangskode:</label>
          <input
            className="w-full p-2 border rounded"
            type={showPassword ? "text" : "password"}
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <button
            type="button"
            className="mt-2 text-blue-500"
            onClick={() => setShowPassword(!showPassword)}
          >
            {showPassword ? "Skjul" : "Vis"}
          </button>
        </div>
        <button type="submit" className="w-full bg-blue-500 text-white p-2 rounded">
          Log ind
        </button>
      </form>
    </div>
  );
};

export default LoginForm;