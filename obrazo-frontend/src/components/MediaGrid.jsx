import MediaCard from './MediaCard';
import '../MediaGrid.css';

function MediaGrid({ medias, name, isUserCard, onCardClick, token}) {
  return (
    <section className="media-grid-section">
      <h2 className="section-title">{name}</h2>
      <div className="media-grid">
        {medias.map(media => (
          <MediaCard key={media.id} media={media} isUserCard={isUserCard} onCardClick={onCardClick} token={token}/>
        ))}
      </div>
    </section>
  );
}

export default MediaGrid;