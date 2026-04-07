import { Link } from 'react-router-dom';
export default function MyPage() {
  return (
    <div>
        <h1>Welcome to job finder service</h1>
        <p>Here you can automate your jobsearch and let a AI assistant go trough the results and match them with your personal skills and cv.</p>
        <p>To get as accurate match as possible please go to the following pages and fill out your information.</p>
        <p><Link to="/my-cv">My CV</Link> to add your cv, either by pasting it or uploading a pdf file.</p>
        <p><Link to="/my-skill-areas">My skill areas</Link> to add specific skills and set a weight from 1 to 5 depending on how important that skill is when the AI assistant does the match.</p>
        <p><Link to="/my-search-terms">My search terms</Link> to add all terms you want to search for.</p>
        <p><Link to="/run-job-match">Run search</Link> to start a job search process</p>
        <p><Link to="/my-job-postings">My joblist</Link> to view your current list of job that the AI assistant has matched with your information.</p>
        <p><Link to="/my-cv">History</Link> to view previous searches and results.</p>
      </div>
  );
}