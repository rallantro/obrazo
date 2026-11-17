import { useState, useEffect } from 'react';
import '../MediaCard.css';

function MediaCard({ media, isUserCard, onCardClick, token }) {
  const [Name, setName] = useState('');
  const [erro, setErro] = useState('');

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

  const pesquisarMedia = async (id) => {
    try {
      const link = `http://localhost:5062/MediaUser/${id}`;
      const response = await fetch(link, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + token
        }
      });
      if (response.ok) {
        const data = await response.json();
        setName(data.media);
        setErro(null);
      } else if (response.status === 404) {
        const msg = await response.json();
        setErro(msg.detail);
      }
    } catch (error) {
      console.error('Erro ao carregar a obra do usuário:', error);
    }
  };

  useEffect(() => {
    if (isUserCard && media) {
      const mediaId = typeof media === 'object' ? media.id : media;
      pesquisarMedia(mediaId);
    }
  }, [media, isUserCard]);


  const currentMediaType = isUserCard && Name ? Name.type : media?.type;

  return (
    <div className="media-card" onClick={() => onCardClick?.(media)}>
      <div className="media-cover">{getEmoji(currentMediaType)}</div>
      <div className="media-info">
        <span className="media-name">
          {!isUserCard && media?.name}
          {isUserCard && Name.name}
        </span>
        {isUserCard && <span className="media-rating">⭐ {media?.nota}</span>}
      </div>
      {erro && <span className="error-text">{erro}</span>}
    </div>
  );
}

export default MediaCard;