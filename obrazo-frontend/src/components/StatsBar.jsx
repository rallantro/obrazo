import '../StatsBar.css';

function StatsBar({ total, mediaNota, favorito }) {
  return (
    <div className="stats-bar">
      <div className="stat-item">
        <span className="stat-number">{total}</span>
        <span className="stat-label">Obras</span>
      </div>
      <div className="stat-item">
        <span className="stat-number">⭐ {mediaNota}</span>
        <span className="stat-label">Média</span>
      </div>
      <div className="stat-item">
        <span className="stat-number">🎬 {favorito?.name}</span>
        <span className="stat-label">Favorito</span>
      </div>
    </div>
  );
}

export default StatsBar;