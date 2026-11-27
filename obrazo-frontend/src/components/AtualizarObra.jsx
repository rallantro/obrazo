import { useState, useEffect } from 'react';
import './AvaliarObra.css';

function AtualizarObra({ media, token, onClose }) {
  const [Name, setName] = useState('');
  const [nota, setNota] = useState(0);
  const [opiniao, setOpin] = useState('');
  const [status, setStatus] = useState(0);
  const [dataAssistiu, setDataA] = useState(new Date().toLocaleDateString('en-CA'));
  const [erro, setErro] = useState('');
  const [sucesso, setSucesso] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErro('');
    setSucesso('');

    try {
      const response = await fetch(`http://localhost:5062/MediaUser/${media.id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify({ nota, opiniao, status, dataAssistiu })
      });

      if (response.ok) {
        setSucesso("Obra atualizada!");
        setTimeout(() => onClose(), 1500)
      } else {
        const msg = await response.text();
        setErro(msg);
      }
    } catch (error) {
      setErro('Erro de conexão com o servidor.');
    }
  };

  const handleDelete = async (x) => {
    x.preventDefault();
    setErro('');
    setSucesso('');
    if (confirm(`Deseja mesmo deletar ${Name.name} da sua coleção?`) == true) {
      try {
        const response = await fetch(`http://localhost:5062/MediaUser/${media.id}`, {
          method: 'DELETE',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
          },
        });
        if (response.ok) {
          setSucesso("Obra excluída.");
          setTimeout(() => onClose(), 1500)
        } else {
          const msg = await response.json();
          setErro(msg);
        }
      } catch (error) {
        setErro('Erro de conexão com o servidor');
      }
    }
  };

  const pesquisarMedia = async (id) => {
    try {
      const link = `http://localhost:5062/MediaUser/${id}`;
      const response = await fetch(link, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + token
        }
      });
      if (response.ok) {
        const data = await response.json();
        setName(data.media);
        setNota(data.nota);
        setOpin(data.opiniao);
        setStatus(data.status);
        setDataA(data.dataAssistiu?.split('T')[0] ?? '');
        setErro(null);
      } else if (response.status === 404) {
        const msg = await response.json();
        setErro(msg.detail);
      }
    } catch (error) {
      console.error('Erro ao carregar a obra do usuário:', error);
    }
  };

  useEffect(() => {
    pesquisarMedia(media.id);
  }, [media.id]);

  return (
    <div className="overlay" onClick={onClose}>
      <div className="modal-card" onClick={(e) => e.stopPropagation()}>
        <button className="modal-close" onClick={onClose}>×</button>
        <h2>Atualizar "{Name.name}"</h2>
        <form onSubmit={handleSubmit}>
          <label>Nota</label>
          <input onChange={(e) => setNota(Number(e.target.value))} type="number" min="0" max="5" step="0.1" value={nota} />

          <label>Opinião</label>
          <textarea value={opiniao} onChange={(e) => setOpin(e.target.value)} />

          <label>Status</label>
          <select value={status} onChange={(e) => setStatus(Number(e.target.value))}>
            <option value={0}>Visto</option>
            <option value={1}>Acompanhando</option>
            <option value={2}>Desejado</option>
          </select>

          <label>Data</label>
          <input required type="date" value={dataAssistiu} onChange={(e) => setDataA(e.target.value)} />

          <button type="submit" disabled={!!sucesso}>
            {sucesso ? 'Salvando...' : 'Atualizar'}
          </button>
          <button type="button" className="delete-button" disabled={!!sucesso} onClick={handleDelete}>
            {sucesso ? '...' : 'Apagar da Coleção'}
          </button>
          {erro && <div className="erro-mensagem">{erro}</div>}
          {sucesso && <div className="sucesso-mensagem">{sucesso}</div>}
        </form>
      </div>
    </div >
  );
}

export default AtualizarObra;