import { useState, useEffect } from 'react';
import '../ReviewCard.css';

function ReviewCard({ media, token }) {
  const [Name, setName] = useState('');
  const [erro, setErro] = useState('');
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
    if (media) {
      const mediaId = typeof media === 'object' ? media.id : media;
      pesquisarMedia(mediaId);
    }
  }, [media]);

  return (
    <div className="review-card">
      <div className="review-poster">🎬</div>
      <div className="review-body">
        <div className="review-header">
          <span className="review-title">{Name.name}</span>
          <span className="review-rating">⭐ {media.nota} / 5</span>
        </div>
        <p className="review-text">"{media.opiniao}"</p>
      </div>
      {erro && <span className="error-text">{erro}</span>}
    </div>
  );
}

export default ReviewCard;