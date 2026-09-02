import Sidebar from '../components/Sidebar';
import TopBar from '../components/TopBar';
import StatsBar from '../components/StatsBar';
import MediaGrid from '../components/MediaGrid';
import ReviewsFeed from '../components/ReviewsFeed';
import '../Dashboard.css';
import { useEffect, useState } from 'react';

function Dashboard({ token, onLogout }) {
  const [user, setUser] = useState('');
  const [erro, setErro] = useState('');
  const [type, setType] = useState(9);
  const [medias, setMedias] = useState([]);

  async function handleFilterChange(newType) {
    setType(newType);
  }

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
  } else if (total === 0) {
    return (
      <div className="dashboard">
        <Sidebar onLogout={onLogout} onFilterChange={handleFilterChange} currentType={type} />
        <div className="main-content">
          <TopBar showButton={true}/>
          <p>Mostrando {type}!</p>
          {erro && <div className="erro-mensagem">{erro}</div>}

        </div>
      </div>
    );
  } else {
    const mediaNota = (medias.reduce((acc, m) => acc + m.nota, 0) / total).toFixed(1);
    const favorito = medias.reduce((a, b) => a.nota > b.nota ? a : b);

    return (
      <div className="dashboard">
        <Sidebar onLogout={onLogout} />
        <div className="main-content">
          <TopBar showButton={true}/>
          <StatsBar total={total} mediaNota={mediaNota} favorito={favorito} />
          <MediaGrid medias={medias.slice(-5).reverse()} />
          <ReviewsFeed medias={medias.filter(m => m.opiniao && m.opiniao.length > 0)} />
        </div>
      </div>
    );

  }

}

export default Dashboard;