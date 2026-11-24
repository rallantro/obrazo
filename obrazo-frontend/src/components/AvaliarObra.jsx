import { useState } from 'react';
import './AvaliarObra.css';

function AvaliarObra({ media, token, onClose }) {
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
      const response = await fetch('http://localhost:5062/MediaUser', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify({ MediaId: media.id, nota, opiniao, status, dataAssistiu })
      });

      if (response.ok) {
        setSucesso("Obra adicionada ao diário!");
        setTimeout(() => onClose(), 1500)
      } else {
        const msg = await response.json()
        setErro(msg.detail);
      }
    } catch (error) {
      setErro('Erro de conexão com o servidor.');
    }
  };


  return (
    <div className="overlay" onClick={onClose}>
      <div className="modal-card" onClick={(e) => e.stopPropagation()}>
        <button className="modal-close" onClick={onClose}>×</button>
        <h2>Avaliar "{media.name}"</h2>
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
            {sucesso ? 'Salvando...' : 'Adicionar ao diário'}
          </button>
          {erro && <div className="erro-mensagem">{erro}</div>}
          {sucesso && <div className="sucesso-mensagem">{sucesso}</div>}
        </form>
      </div>
    </div >
  );
}

export default AvaliarObra;