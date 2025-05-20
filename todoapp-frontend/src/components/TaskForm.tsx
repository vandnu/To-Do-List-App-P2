import React, { useState } from "react";

interface TaskFormProps {
  onSubmit: (taskData: { title: string; description?: string; isCompleted: boolean }) => void;
}

const TaskForm: React.FC<TaskFormProps> = ({ onSubmit }) => {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [isCompleted, setIsCompleted] = useState(false);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit({ title, description: description || undefined, isCompleted });
    setTitle("");
    setDescription("");
    setIsCompleted(false);
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4 mb-6">
      <div>
        <label className="block mb-1">Titel:</label>
        <input
          className="w-full p-2 border rounded"
          type="text"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          placeholder="Titel"
          required
        />
      </div>
      <div>
        <label className="block mb-1">Beskrivelse (valgfri):</label>
        <input
          className="w-full p-2 border rounded"
          type="text"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          placeholder="Beskrivelse"
        />
      </div>
      <label className="block">
        <input
          type="checkbox"
          checked={isCompleted}
          onChange={(e) => setIsCompleted(e.target.checked)}
        />
        <span className="ml-2">Færdig?</span>
      </label>
      <button type="submit" className="w-full bg-blue-500 text-white p-2 rounded">
        Opret
      </button>
    </form>
  );
};

export default TaskForm;