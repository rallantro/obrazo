import { useState, useEffect } from 'react'; 

function FormularioMidia() {
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

      if (response.ok) {
        setMensagem('Mídia adicionada com sucesso!');
        setNome('');
        carregarMedias(); 
      } else {
        setMensagem('Erro ao adicionar mídia.');
      }
    } catch (error) {
      console.error(error);
      setMensagem('Erro de conexão.');
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <h3>Adicionar nova mídia</h3>

      <label>Nome:</label>
      <input
        type="text"
        value={nome}
        onChange={(e) => setNome(e.target.value)}
        placeholder="Ex: Interestelar"
      />

      <label>Tipo:</label>
      <select value={tipo} onChange={(e) => setTipo(Number(e.target.value))}>
        <option value={0}>LIVRO</option>
        <option value={1}>SERIE</option>
        <option value={2}>ANIME</option>
        <option value={3}>FILME</option>
        <option value={4}>MANGA</option>
        <option value={5}>DESENHO</option>
      </select>

      <button type="submit">Adicionar</button>

      {mensagem && <p>{mensagem}</p>}

      <h2>Lista de Mídias</h2>
      <ul>
        {medias.map(media => (
          <li key={media.id}>
            <strong>{media.name}</strong> - Tipo: {media.type}
          </li>
        ))}
      </ul>
    </form>
  );
}

export default FormularioMidia;