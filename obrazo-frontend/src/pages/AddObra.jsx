import { useState, useEffect } from 'react';
import '../AddObra.css';

function AddObra({ token, onLogout }) {
  const [nome, setNome] = useState('');
  const [tipo, setTipo] = useState(0);
  const [mensagem, setMensagem] = useState('');
  const [medias, setMedias] = useState([]);

  const carregarMedias = async () => {
    try {
      const response = await fetch('http://localhost:5062/Media');
      const data = await response.json();
      setMedias(data);
    } catch (error) {
      if (response.status === 401) {
        localStorage.removeItem('token');
        onLogout();
        return;
      }
      console.error('Erro ao carregar mídias:', error);
    }
  };


  useEffect(() => {
    carregarMedias();
  }, []);

  const handleSubmit = async (event) => {
    event.preventDefault();

    const novaMidia = { name: nome, type: tipo };

    try {
      const response = await fetch('http://localhost:5062/Media', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify(novaMidia)
      });
      if (response.status === 401) {
        localStorage.removeItem('token');
        onLogout();
        return;
      }
      if (response.ok) {
        setMensagem('Mídia adicionada com sucesso!');
        setNome('');
        carregarMedias();
      } else {
        const msg = await response.text();
        setMensagem(msg);
      }
    } catch (error) {
      console.error(error);
      setMensagem("Erro de conexão");
    }
  };

  return (
    <div className="add-container">
      <div className="add-card">
        <h2>Nova Obra</h2>
        <p className="subtitle">Cadastre uma nova mídia ao catálogo</p>

        <form onSubmit={handleSubmit}>
          <label>Nome da obra</label>
          <input
            type="text"
            value={nome}
            onChange={(e) => setNome(e.target.value)}
            placeholder="Ex: O Pretendente"
            required
          />

          <label>Tipo</label>
          <select value={tipo} onChange={(e) => setTipo(Number(e.target.value))}>
            <option value={0}>LIVRO</option>
            <option value={1}>SERIE</option>
            <option value={2}>ANIME</option>
            <option value={3}>FILME</option>
            <option value={4}>MANGÁ</option>
            <option value={5}>DESENHO</option>
            <option value={6}>JOGO</option>
          </select>

          <button type="submit" className="btn-submit">Adicionar ao catálogo</button>
        </form>

        {mensagem && (
          <div className={mensagem.includes('sucesso') ? 'mensagem-sucesso' : 'mensagem-erro'}>
            {mensagem}
          </div>
        )}

        <div className="lista-medias">
          <h3>Mídias cadastradas</h3>
          <ul>
            {medias.map(media => (
              <li key={media.id}>
                <strong>{media.name}</strong>
                <span className="tipo-badge">
                  {['LIVRO', 'SERIE', 'ANIME', 'FILME', 'MANGA', 'DESENHO', 'JOGO'][media.type] || media.type}
                </span>
              </li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
}

export default AddObra;