import React from "react";
import { BrowserRouter as Router, Routes, Route, Link, Navigate } from "react-router-dom";
import LoginForm from "./components/LoginForm";
import RegisterForm from "./components/RegisterForm";
import TaskList from "./components/TaskList";
import QuoteList from "./components/QuoteList";
import { useAuthStore } from "./store/authStore";

const App: React.FC = () => {
  const { token, logout } = useAuthStore();

  return (
    <Router>
      <nav>
        <Link to="/login">Login</Link> | <Link to="/register">Register</Link> |{" "}
        <Link to="/tasks">Tasks</Link> | <Link to="/quotes">Quotes</Link>
        {token && <button onClick={logout}>Logout</button>}
      </nav>
      <Routes>
        <Route path="/login" element={<LoginForm />} />
        <Route path="/register" element={<RegisterForm />} />
        <Route path="/tasks" element={token ? <TaskList /> : <Navigate to="/login" />} />
        <Route path="/quotes" element={<QuoteList />} />
        <Route path="/" element={<Navigate to="/login" />} />
      </Routes>
    </Router>
  );
};

export default App;