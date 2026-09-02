import { useState, useEffect } from 'react';
import TopBar from '../components/TopBar';
import MediaGrid from '../components/MediaGrid';

function Buscar({ token, onLogout }) {
  const [search, setSearch] = useState('');
  const [erro, setErro] = useState('');
  const [medias, setMedias] = useState([]);
  const mediasOrdenadas = [...medias].sort((a, b) => a.name.localeCompare(b.name));

  async function handleSearchChange(newSearch) {
    setSearch(newSearch);
  }


  const pesquisarMedias = async (name) => {
    try {
      let link = 'http://localhost:5062/Media';
      if (name != '') {
        link = 'http://localhost:5062/Media/title/' + name;
      }
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
  }, [search]);

  return (
    <div className="dashboard">
      <div className="main-content">
        <TopBar showButton={false} onSearchChange={handleSearchChange}/>
        <MediaGrid medias={medias.slice(-5).reverse()}  name="Últimas adicionadas" isUserCard={false}/>
        <MediaGrid medias={mediasOrdenadas} name="Todas as Obras" isUserCard={false} />
      </div>
    </div>
  );
}

export default Buscar;