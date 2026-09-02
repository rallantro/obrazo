import '../MediaCard.css';

function MediaCard({ media,  isUserCard}) {
  const getEmoji = (type) => {
    const emojis = {
      0: '📚',   
      1: '📺',   
      2: '🎌',   
      3: '🎬',   
      4: '📖',   
      5: '🎨',   
      6: '🎮',   
    };
    return emojis[type] || '🎬'; 
  };

  return (
    <div className="media-card">
      <div className="media-cover">{getEmoji(media.type)}</div>
      <div className="media-info">
        <span className="media-name">{media.name}</span>
        {isUserCard && <span className="media-rating">⭐ {media.nota}</span>}
      </div>
    </div>
  );
}

export default MediaCard;