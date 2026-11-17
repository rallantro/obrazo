import ReviewCard from './ReviewCard';
import '../ReviewsFeed.css';

function ReviewsFeed({ medias, token}) {
  if (medias.length === 0) {
    return <p className="no-reviews">Nenhuma review ainda. Adicione uma opinião!</p>;
  }

  return (
    <section className="reviews-feed">
      <h2 className="section-title">Reviews recentes</h2>
      {medias.map(media => (
        <ReviewCard key={media.id} media={media} token={token}/>
      ))}
    </section>
  );
}

export default ReviewsFeed;