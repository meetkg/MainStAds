
let allBusinesses = [];
const itemsPerPage = 6;
let currentPage = 1;

async function fetchAllBusinesses() {
    const response = await fetch("/Home/GetBusinesses");
    allBusinesses = await response.json();
    handleFilterAndSort();
    populateCategories();
    updateArrows();
}

function handleFilterAndSort() {
    const searchQuery = document.getElementById('searchBox').value.toLowerCase();
    const selectedCategory = document.getElementById('categoryDropdown').value;
    const showNearby = document.getElementById('showNearby').checked;

    let filteredBusinesses = [...allBusinesses];

    if (searchQuery) {
        filteredBusinesses = filteredBusinesses.filter(business => business.name.toLowerCase().includes(searchQuery));
    }

    if (selectedCategory) {
        filteredBusinesses = filteredBusinesses.filter(business => business.category === selectedCategory);
    }

    if (showNearby) {
        navigator.geolocation.getCurrentPosition(position => {
            const userLatitude = position.coords.latitude;
            const userLongitude = position.coords.longitude;
            document.getElementById('locationInfo').innerText = `${userLatitude}, ${userLongitude}`;

            filteredBusinesses.forEach(business => {
                business.distance = calculateDistance(userLatitude, userLongitude, business.latitude, business.longitude);
            });
            filteredBusinesses.sort((a, b) => (a.distance || 0) - (b.distance || 0));
            displayBusinesses(filteredBusinesses);
        });
    } else {
        displayBusinesses(filteredBusinesses);
    }
}

function calculateDistance(lat1, lon1, lat2, lon2) {
    const earthRadiusKm = 6371.0;
    const radiansLat1 = (Math.PI * lat1) / 180.0;
    const radiansLon1 = (Math.PI * lon1) / 180.0;
    const radiansLat2 = (Math.PI * lat2) / 180.0;
    const radiansLon2 = (Math.PI * lon2) / 180.0;

    const deltaLat = radiansLat2 - radiansLat1;
    const deltaLon = radiansLon2 - radiansLon1;

    const a = Math.sin(deltaLat / 2) * Math.sin(deltaLat / 2) + Math.cos(radiansLat1) * Math.cos(radiansLat2) * Math.sin(deltaLon / 2) * Math.sin(deltaLon / 2);
    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    const distance = earthRadiusKm * c;

    return distance;
}

function displayBusinesses(businesses) {
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    const displayedBusinesses = businesses.slice(startIndex, endIndex);

    let html = '';
    for (const business of displayedBusinesses) {
        const formattedLink = (business.link.startsWith('http://') || business.link.startsWith('https://')) ? business.link : 'https://' + business.link;

        html += `<div class="business-card">`;
        if (business.imageData) {
            html += `<img src="data:image/${business.imageType};base64,${business.imageData}" alt="${business.name}" />`;
        }

        html += `<h3><a href="${formattedLink}" target="_blank" rel="noopener noreferrer">${business.name}</a></h3>
                                 <p>Category: ${business.category}</p>
                                 <p>Description: ${business.description}</p>
                                 <p>Address: ${business.address}</p>
                                 <p>Distance: ${business.distance ? business.distance.toFixed(2) + ' km' : 'N/A'}</p>
                                 </div>`;
    }
    document.getElementById('businessesContainer').innerHTML = html;
    displayPaginationControls(businesses.length);
}

function displayPaginationControls(totalItems) {
    const totalPages = Math.ceil(totalItems / itemsPerPage);
    let paginationHtml = '<button id="prevArrow" class="arrow-btn" onclick="prevPage()">❮</button>';
    for (let i = 1; i <= totalPages; i++) {
        paginationHtml += `<button class="${i === currentPage ? 'active-page' : ''}" onclick="changePage(${i})">${i}</button>`;
    }
    paginationHtml += '<button id="nextArrow" class="arrow-btn" onclick="nextPage()">❯</button>';

    document.getElementById('paginationControls').innerHTML = paginationHtml;
}

function populateCategories() {
    const categorySet = new Set();
    allBusinesses.forEach(business => categorySet.add(business.category));
    const dropdown = document.getElementById('categoryDropdown');
    Array.from(categorySet).forEach(category => {
        const option = document.createElement('option');
        option.value = category;
        option.innerText = category;
        dropdown.appendChild(option);
    });
}

function changePage(pageNumber) {
    currentPage = pageNumber;
    handleFilterAndSort();
    updateArrows();
}

function nextPage() {
    if (currentPage * itemsPerPage < allBusinesses.length) {
        changePage(currentPage + 1);
    }
}

function prevPage() {
    if (currentPage > 1) {
        changePage(currentPage - 1);
    }
}

function updateArrows() {
    document.getElementById('prevArrow').disabled = currentPage === 1;
    document.getElementById('prevArrowBig').disabled = currentPage === 1;

    document.getElementById('nextArrow').disabled = currentPage * itemsPerPage >= allBusinesses.length;
    document.getElementById('nextArrowBig').disabled = currentPage * itemsPerPage >= allBusinesses.length;
}

document.getElementById('searchBox').addEventListener('input', () => { currentPage = 1; handleFilterAndSort(); });
document.getElementById('categoryDropdown').addEventListener('change', () => { currentPage = 1; handleFilterAndSort(); });
document.getElementById('showNearby').addEventListener('change', () => { currentPage = 1; handleFilterAndSort(); });

fetchAllBusinesses();