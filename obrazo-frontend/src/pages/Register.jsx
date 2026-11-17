import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';

function Register() {
    const [nome, setNome] = useState('');
    const [senha, setSenha] = useState('');
    const [confirmarSenha, setCSenha] = useState('');
    const [erro, setErro] = useState('');
    const [sucesso, setSucesso] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErro('');
        setSucesso('');

        if (senha !== confirmarSenha) {
            setErro("As senhas não conferem!");
            return;
        }
        if(senha.length < 6){
            setErro("A senha precisa ter no mínimo 6 caracteres!");
            return;
        }
        if(!/\d/.test(senha)){
            setErro("A senha precisa ter no mínimo 1 número!");
            return;
        }

        try {
            const response = await fetch('http://localhost:5062/User', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ name: nome, password: senha })
            });

            if (response.ok) {
                setSucesso("Conta criada com sucesso!");
                setTimeout(() => navigate('/'), 1500)
            } else {
                const msg = await response.text();
                setErro(msg);
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
                    <h2>Cadastro</h2>
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
                        <label>Confirme a Senha:</label>
                        <input
                            type="password"
                            value={confirmarSenha}
                            onChange={(e) => setCSenha(e.target.value)}
                            placeholder="Confirme sua senha"
                            required
                        />
                    </div>
                    {erro && <div className="erro-mensagem">{erro}</div>}
                    {sucesso && <div className="sucesso-mensagem">{sucesso}</div>}
                    <button type="submit" className="btn-login" disabled={!!sucesso}>{sucesso ? 'Cadastrando...' : 'Criar uma conta'}</button>
                    <p className="register-link">
                        Já possui uma conta? <Link to="/">Faça login agora!</Link>
                    </p>
                </form>
            </div>
        </div>
    );
}

export default Register;