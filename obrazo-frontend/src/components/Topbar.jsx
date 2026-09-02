import { FiSearch, FiPlus, FiUser, FiPlayCircle, FiPlusCircle } from 'react-icons/fi';
import '../TopBar.css';
import { Link } from 'react-router-dom';



function TopBar({showButton, onSearchChange}) {
  return (
    <div className="topbar">
      <div className="search-wrapper">
        <FiSearch className="search-icon" />
        <input type="text" placeholder="Pesquisar obras..." className="search-input" onChange={(e) => onSearchChange(e.target.value)}/>
      </div>
      <div className="topbar-actions">
        <> {showButton ? <Link to="/buscar" className="btn-add"><FiSearch />Procurar Obras</Link> : <Link to="/adicionar" className="btn-add"><FiPlusCircle />Adicionar Obras</Link>} </>
      </div>
    </div>
  );
}

export default TopBar;