import { useState } from 'react';

function Home({ onLogin }) {
  const [nome, setNome] = useState('');
  const [senha, setSenha] = useState('');
  const [erro, setErro] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErro('');

    try {
      const response = await fetch('http://localhost:5062/Auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name: nome, password: senha })
      });

      if (response.ok) {
        const data = await response.json();
        localStorage.setItem('token', data.token);
        onLogin(data.token);
      } else {
        const msg = await response.text();
        setErro(msg || 'Usuário ou senha inválidos.');
      }
    } catch (error) {
      setErro('Erro de conexão com o servidor.');
    }
  };

  return (
    <div className="home-container">
      <div className="home-left">
        <h1 className="title">
          Obrazo<span className="accent">.</span>
        </h1>
        <p className="subtitle">Seu acervo pessoal de histórias inesquecíveis.</p>
        <p className="description">
          Organize filmes, séries, livros e jogos em um só lugar. Registre suas impressões, dê notas e acompanhe sua jornada cultural.
        </p>
      </div>
      <div className="home-right">
        <form onSubmit={handleSubmit} className="login-form">
          <h2>Login</h2>
          <div className="form-group">
            <label>Nome de usuário</label>
            <input
              type="text"
              value={nome}
              onChange={(e) => setNome(e.target.value)}
              placeholder="Digite seu nome"
              required
            />
          </div>
          <div className="form-group">
            <label>Senha</label>
            <input
              type="password"
              value={senha}
              onChange={(e) => setSenha(e.target.value)}
              placeholder="Digite sua senha"
              required
            />
          </div>
          {erro && <div className="erro-mensagem">{erro}</div>}
          <button type="submit" className="btn-login">Entrar</button>
          <p className="register-link">
            Ainda não tem conta? <a href="#">Crie uma agora</a> (em breve)
          </p>
        </form>
      </div>
    </div>
  );
}

export default Home;