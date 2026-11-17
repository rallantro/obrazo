import { useState, useEffect } from 'react';
import TopBar from '../components/TopBar';
import MediaGrid from '../components/MediaGrid';
import Sidebar from '../components/Sidebar';
import AvaliarObra from '../components/AvaliarObra';

function Buscar({ token, onLogout }) {
  const [search, setSearch] = useState('');
  const [erro, setErro] = useState('');
  const [type, setType] = useState(9);
  const [medias, setMedias] = useState([]);
  const mediasOrdenadas = [...medias].sort((a, b) => a.name.localeCompare(b.name));
  const [obraSelecionada, setObraSelecionada] = useState(null);

  async function handleSearchChange(newSearch) {
    setSearch(newSearch);
  }

  async function handleFilterChange(newType) {
    setType(newType);
  }


  const pesquisarMedias = async (name) => {
    try {
      const params = new URLSearchParams();
      if (search) params.append('name', search);
      if (type !== 9) params.append('type', type);

      const link = `http://localhost:5062/Media/search?${params.toString()}`;
      const response = await fetch(link, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + token
        }
      });
      if (response.ok) {
        const data = await response.json();
        setMedias(data);
        setErro(null);
      } else if (response.status === 404) {
        const msg = await response.json();
        setErro(msg.detail);
      }
    } catch (error) {
      console.error('Erro ao carregar as obras:', error);
    }
  };


  useEffect(() => {
    pesquisarMedias(search);
  }, [search, type]);

  return (
    <div className="dashboard">
      {obraSelecionada && <AvaliarObra media={obraSelecionada} token={token} onClose={() => setObraSelecionada(null)} />}
      <Sidebar onLogout={onLogout} onFilterChange={handleFilterChange} currentType={type} />
      <div className="main-content">
        <TopBar showButton={false} onSearchChange={handleSearchChange} />
        {!erro && (
          <>
            <MediaGrid medias={medias.slice(-6).reverse()} name="Últimas adicionadas" isUserCard={false} onCardClick={setObraSelecionada} />
            <MediaGrid medias={mediasOrdenadas} name="Todas as Obras" isUserCard={false} onCardClick={setObraSelecionada} />
          </>
        )}
      </div>
    </div>
  );
}

export default Buscar;