const express = require('express');
const fetch = require('node-fetch'); // node-fetch v2 supports require
const cors = require('cors');

console.log('Starting server...');

const app = express();
const PORT = process.env.PORT || 3000;

app.use(cors());

app.get('/jobs', async (req, res) => {
  try {
    const page = req.query.page || 1;
    const apiUrl = `https://arbeitnow.com/api/job-board-api?count=20&page=${page}`;

    const response = await fetch(apiUrl);
    if (!response.ok) {
      return res.status(response.status).json({ error: 'Error fetching jobs' });
    }
    const data = await response.json();
    res.json(data);
  } catch (error) {
    console.error('Fetch error:', error.message);
    res.status(500).json({ error: 'Internal Server Error' });
  }
});

app.listen(PORT, () => {
  console.log(`Proxy server running on http://localhost:${PORT}`);
});