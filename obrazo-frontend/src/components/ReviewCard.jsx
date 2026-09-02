import '../ReviewCard.css';

function ReviewCard({ media }) {
  return (
    <div className="review-card">
      <div className="review-poster">🎬</div>
      <div className="review-body">
        <div className="review-header">
          <span className="review-title">{media.name}</span>
          <span className="review-rating">⭐ {media.nota} / 5</span>
        </div>
        <p className="review-text">"{media.opiniao}"</p>
      </div>
    </div>
  );
}

export default ReviewCard;