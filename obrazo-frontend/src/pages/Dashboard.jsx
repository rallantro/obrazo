import Sidebar from '../components/Sidebar';
import TopBar from '../components/TopBar';
import StatsBar from '../components/StatsBar';
import MediaGrid from '../components/MediaGrid';
import ReviewsFeed from '../components/ReviewsFeed';
import AtualizarObra from '../components/AtualizarObra';
import '../Dashboard.css';
import { useEffect, useState } from 'react';

function Dashboard({ token, onLogout }) {
  const [search, setSearch] = useState('');
  const [user, setUser] = useState('');
  const [erro, setErro] = useState('');
  const [type, setType] = useState(9);
  const [medias, setMedias] = useState([]);
  const [obraSelecionada, setObraSelecionada] = useState(null);


  async function handleSearchChange(newSearch) {
    setSearch(newSearch);
  }
  const pesquisarMedias = async (name) => {
    try {
      const params = new URLSearchParams();
      if (search) params.append('name', search);
      if (type !== 9) params.append('type', type);

      const link = `http://localhost:5062/MediaUser/search?${params.toString()}`;
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
        setMedias([]);
        const msg = await response.json();
        setErro(msg.detail);
      }
    } catch (error) {
      console.error('Erro ao carregar as obras:', error);
    }
  };

  async function handleFilterChange(newType) {
    setType(newType);
  }

  useEffect(() => {
    pesquisarMedias(search);
  }, [search, type]);

  useEffect(() => {
    async function buscarDados() {
      setErro('');

      try {
        const responseUser = await fetch('http://localhost:5062/User/me', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
          }
        });

        if (responseUser.ok) {
          const userName = await responseUser.text();
          console.log('Nome recebido:', userName);
          setUser(userName);
        } else if (responseUser.status === 404) {
          setErro('Você não está presente na lista de usuários. Isso é um erro incomum, por favor reporte.');
        } else if (response.status === 401) {
          localStorage.removeItem('token');
          onLogout();
          return;
        }
      } catch (error) {
        setErro('Erro de conexão com o servidor.');
      }
      try {
        let link = 'http://localhost:5062/MediaUser/user/';
        if (type != '9') {
          link = 'http://localhost:5062/MediaUser/type/' + type;
        }
        const response = await fetch(link, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
          },
        });

        if (response.ok) {
          const medias = await response.json();
          setMedias(medias);
        } else if (response.status === 404) {
          const msg = await response.json();
          setMedias([]);
          setErro(msg.detail);
        } else if (response.status === 401) {
          localStorage.removeItem('token');
          onLogout();
          return;
        }
      } catch (error) {
        setErro('Erro de conexão com o servidor.');
      }
    }

    buscarDados();
  }, [type]);



  const total = medias.length;


  if (user === '') {
    return (
      <div className="dashboard">
        <Sidebar onLogout={onLogout} />
        <div className="main-content">
          <TopBar showButton={true} />
          {erro && <div className="erro-mensagem">{erro}</div>}
        </div>
      </div>
    );
  } else {
    let mediaNota = 0;
    let favorito = null;
    if (total > 0) {
      mediaNota = (medias.reduce((acc, m) => acc + m.nota, 0) / total).toFixed(1);
      favorito = medias.reduce((a, b) => a.nota > b.nota ? a : b);
    }
    return (
      <div className="dashboard">
        {obraSelecionada && <AtualizarObra media={obraSelecionada} token={token} onClose={() => setObraSelecionada(null)} />}
        <Sidebar onLogout={onLogout} onFilterChange={handleFilterChange} currentType={type} onSearch={false} />
        <div className="main-content">
          <TopBar showButton={true} onSearchChange={handleSearchChange} />
          {erro && <div className="erro-mensagem">{erro}</div>}
          {total > 0 && (<><StatsBar total={total} mediaNota={mediaNota} favorito={favorito} />
            <MediaGrid medias={medias.toReversed()} isUserCard={true} onCardClick={setObraSelecionada} token={token} />
            <ReviewsFeed medias={medias.filter(m => m.opiniao && m.opiniao.length > 0).reverse()} token={token} /></>)}
        </div>
      </div>
    );

  }

}

export default Dashboard;