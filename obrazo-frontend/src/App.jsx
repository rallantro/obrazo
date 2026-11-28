import { useState } from 'react';
import Home from './pages/Home';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import { Routes, Route, Navigate } from 'react-router-dom';
import AddObra from './pages/AddObra';
import Profile from './pages/Profile';
import './App.css';
import Buscar from './pages/Buscar';

function App() {
  const [token, setToken] = useState(localStorage.getItem('token') || null);

  const handleLogin = (novoToken) => {
    setToken(novoToken);
  };


  const handleLogout = () => {
    localStorage.removeItem('token');
    setToken(null);
  };



  return (
    <div className="App">
      <Routes>
        <Route
          path="/"
          element={
            !token ? (
              <div className="home-page">
                <Home onLogin={handleLogin} />
              </div>
            ) : (
              <Dashboard token={token} onLogout={handleLogout} />
            )
          }
        />
        <Route
          path='/cadastro'
          element={
            !token ? (
              <div className="home-page">
                <Register/>
              </div>
            ) : (
              <Dashboard token={token} onLogout={handleLogout} />
            )
          }
        />
        <Route
          path="/buscar"
          element={
            !token ? (
              <Navigate to="/" replace />
            ) : (
              <Buscar token={token} onLogout={handleLogout} />
            )
          }
        />
        <Route
          path="/adicionar"
          element={
            !token ? (
              <Navigate to="/" replace />
            ) : (
              <AddObra token={token} />
            )
          }
        />
        <Route
          path="/profile"
          element={
            !token ? (
              <Navigate to="/" replace />
            ) : (
              <Profile token={token} />
            )
          }
        />
      </Routes>
    </div>
  );
}

export default App;