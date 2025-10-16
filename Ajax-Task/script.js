/*fetch('http://localhost:3000/jobs')
  .then(r => r.json())
  .then(data => console.log(data))
  .catch(console.error);*/

const btnLoad = document.getElementById("loader");


let currentPage = 1;

btnLoad.addEventListener('click', async () => {
  await fetchJobs(currentPage);
});

document.addEventListener('click', function(e) {
      if (e.target.classList.contains('fetch-next')) {
          fetchJobs(++currentPage);
      }
      if (e.target.classList.contains('fetch-prev')) {
          fetchJobs(--currentPage);
      }
      if (e.target.classList.contains('fetch-page')) {
          currentPage = e.target.textContent;
          fetchJobs(currentPage);
      }
});

async function fetchJobs(page) {
  try {
    const response = await fetch(`http://localhost:3000/jobs?page=${page}`);

    if (response.ok) {
      const data = await response.json();

      console.log(data);

      if (!document.querySelector('.job-list')) {
        const jobListElement = document.createElement('div');
        jobListElement.classList.add('job-list', 'container-fluid', 'mt-4');

        const rowDiv = document.createElement('div');
        rowDiv.classList.add('row');
        jobListElement.appendChild(rowDiv);

        document.body.appendChild(jobListElement);
      }

      loadJobs(data.data);
      renderPaginationControls(data.links, data.meta);
    }
    else {
      throw new Error("Error occured:" + response.status + " status:" + response.statusText);
    }
  }
  catch (error) {
    console.error('Fetch', error);
  }
}

function loadJobs(jobsAsJson) {
  const jobListElement = document.querySelector('.job-list');
  jobListElement.innerHTML = '';

  for (const job of jobsAsJson) {
    const jobDiv = document.createElement('div');
    jobDiv.classList.add('card', 'mb-3', 'shadow-sm');

    const jobTitle = document.createElement('h5');
    jobTitle.classList.add('card-title');
    jobTitle.setAttribute('data-bs-toggle', 'modal');
    jobTitle.setAttribute('data-bs-target', `#${job.slug}`);
    jobTitle.addEventListener('mouseover', (event) => {
      event.target.style.textDecoration = 'underline';
    });
    jobTitle.addEventListener('mouseout', (event) => {
      event.target.style.textDecoration = 'none';
    });
    jobTitle.textContent = job.title;

    const jobAddedAt = document.createElement('p');
    jobAddedAt.classList.add('card-text');
    jobAddedAt.textContent = new Intl.DateTimeFormat('bg-BG', {
      dateStyle: 'medium',
      timeStyle: 'short',
    }).format(job.created_at);

    if (!document.getElementById(job.slug)) {
      const jobModalDiv = document.createElement('div');
      jobModalDiv.classList.add('modal', 'fade');
      jobModalDiv.setAttribute('id', job.slug);
      jobModalDiv.setAttribute('tabindex', '-1');
      jobModalDiv.setAttribute('aria-labelledby', 'jobDetails');
      jobModalDiv.setAttribute('aria-hidden', 'true');

      const parser = new DOMParser();
      const htmlDoc = parser.parseFromString(job.description, 'text/html');
      let regex = /\n*\s*<iframe.*?\\?>.*?<\/iframe\\?>\s*\n*/gi;

      // const iframe = htmlDoc.querySelector('iframe');
      // if (iframe) {
      //   iframe.removeAttribute('width');
      //   iframe.removeAttribute('height');

      //   const wrapper = document.createElement('div');
      //   wrapper.classList.add('ratio', 'ratio-16x9');
      //   iframe.parentNode.replaceChild(wrapper, iframe);
      //   wrapper.appendChild(iframe);
      // }


      jobModalDiv.innerHTML = `<div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header flex-column align-items-start">
            <div class="w-100 d-flex justify-content-end">
              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>

            <h1 class="modal-title fs-5 mb-1" id="jobDetails">${job.title}</h1>
            <h2 class="fs-6 text-muted">Job listed by ${job.company_name}</h2>
          </div>
          <div class="modal-body">
            ${htmlDoc.body.innerHTML.replace(regex, '')}
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
            <a class="btn btn-outline-primary" href=${job.url} target="_blank">Go to job listing</a>
          </div>
        </div>
      </div>`;

      document.body.appendChild(jobModalDiv);
    }
    const jobDetailsDiv = document.createElement('div');
    jobDetailsDiv.classList.add('d-grid', 'gap-2', 'd-md-flex', 'justify-content-md-end');

    jobDiv.appendChild(jobDetailsDiv);
    jobDiv.appendChild(jobTitle);
    jobDiv.appendChild(jobAddedAt);

    jobListElement.appendChild(jobDiv);
  };
}


function renderPaginationControls(links, meta) {
  const paginationContainer = document.getElementById('paginationControls');
  paginationContainer.innerHTML = '';
  if (!document.getElementById('prev')) {
    const prevBtn = document.createElement('button');
    prevBtn.textContent = 'Previous';
    prevBtn.classList.add('btn', 'btn-outline-primary', 'me-2', "fetch-prev");
    // prevBtn.setAttribute('data-page', meta.current_page - 1); /no longer used/
    prevBtn.id = 'prev';
    paginationContainer.appendChild(prevBtn);
  }
  else {
    const prevBtn = document.getElementById('prev');
    prevBtn.setAttribute('data-page', meta.current_page - 1);
    // prevBtn.addEventListener('click', () => {
    //   if (links.prev) {
    //     currentPage--;
    //     fetchJobs(currentPage);
    //   }
    // });
  }

  for (let i = 0; i<10; i++)
  {
    pageel = document.createElement('span');    
    pageel.classList.add('btn', 'btn-outline-primary', 'me-2', "fetch-page");
    pageel.textContent = i+1;
    paginationContainer.append(pageel);
  }


  if (!document.getElementById('next')) {
    const nextBtn = document.createElement('button');
    nextBtn.textContent = 'Next';
    nextBtn.classList.add('btn', 'btn-outline-primary', 'me-2', "fetch-next");
    // nextBtn.setAttribute('data-page', meta.current_page + 1); /no longer used/
    nextBtn.id = 'next';
    paginationContainer.appendChild(nextBtn);
  }
  else {
    const prevBtn = document.getElementById('next');
    prevBtn.setAttribute('data-page', meta.current_page + 1);
    // console.log(document.getElementById('next'));
    // nextBtn.addEventListener('click', () => {
    //   if (links.next) {
    //     currentPage++;
    //     fetchJobs(currentPage);
    //   }
    // });
  }
  
  const currentPageSpan = document.createElement('p');
  currentPageSpan.textContent = `Page ${meta.current_page}`;
  currentPageSpan.classList.add('mx-2');
  currentPageSpan.classList.add('text-center');
  paginationContainer.appendChild(currentPageSpan);

  const jobListElement = document.querySelector('.job-list');
  //jobListElement.innerHTML = '';

  /*if (links.prev) {
    const prevBtn = document.createElement('button');
    prevBtn.textContent = 'Previous';
    prevBtn.classList.add('btn', 'btn-outline-primary', 'me-2');
    prevBtn.addEventListener('click', () => {
      currentPage = meta.current_page - 1;
      fetchJobs(currentPage);
    });
    paginationContainer.appendChild(prevBtn);
  }*/

  /*if (links.next) {
    const nextBtn = document.createElement('button');
    nextBtn.textContent = 'Next';
    nextBtn.classList.add('btn', 'btn-outline-primary', 'ms-2');
    nextBtn.addEventListener('click', () => {
      currentPage = meta.current_page + 1;
      fetchJobs(currentPage);
    });
    paginationContainer.appendChild(nextBtn);
  }*/
}

//https://pokeapi.co/api/v2/pokemon/ditto
//https://arbeitnow.com/api/job-board-api