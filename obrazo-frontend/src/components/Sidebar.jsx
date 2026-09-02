import '../Sidebar.css';
import LogoIcon from './LogoIcon';



function Sidebar({ onLogout, onFilterChange, currentType }) {

  return (
    <aside className="sidebar">
      <div className="logo" href="#">
        <LogoIcon/>
        <span className="logo-text">Obrazo</span>
      </div>
      <nav className="menu">
        <a href="#" onClick={() => onFilterChange(9)} className={`menu-item ${currentType === 9 ? 'active' : ''}`}>Início</a>
        <span className="menu-category">Categorias</span>
        <a href="#" onClick={() => onFilterChange(3)} className={`menu-item ${currentType === 3 ? 'active' : ''}`}>Filme</a>
        <a href="#" onClick={() => onFilterChange(1)} className={`menu-item ${currentType === 1 ? 'active' : ''}`}>Série</a>
        <a href="#" onClick={() => onFilterChange(0)} className={`menu-item ${currentType === 0 ? 'active' : ''}`}>Livro</a>
        <a href="#" onClick={() => onFilterChange(6)} className={`menu-item ${currentType === 6 ? 'active' : ''}`}>Jogo</a>
        <a href="#" onClick={() => onFilterChange(2)} className={`menu-item ${currentType === 2 ? 'active' : ''}`}>Anime</a>
        <a href="#" onClick={() => onFilterChange(4)} className={`menu-item ${currentType === 4 ? 'active' : ''}`}>Mangá</a>
        <a href="#" onClick={() => onFilterChange(5)} className={`menu-item ${currentType === 5 ? 'active' : ''}`}>Desenho</a>
      </nav>
      <div className="menu-bottom">
        <a href="#" className="menu-item">Perfil</a>
        <a href="#" onClick={onLogout} className="menu-item logout"> Sair</a>
      </div>
    </aside>
  );
}

export default Sidebar;