


// Function to fetch all books
document.addEventListener("DOMContentLoaded", () => {
    console.log("DOMContentLoaded event fired"); // Debug log
    fetchBooks();
});

function fetchBooks() {
    console.log("fetchBooks function called"); // Debug log

    const bookListDiv = document.getElementById('bookList');
    bookListDiv.innerHTML = '<p>Loading...</p>'; // Loading indicator

    fetch('/api/books')
        .then(response => {
            console.log("Fetch request made to /api/books"); // Debug log

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(books => {
            console.log("Books data fetched successfully"); // Debug log
            bookListDiv.innerHTML = ''; // Clear loading text

            books.forEach(book => {
                const bookDiv = document.createElement('div');
                bookDiv.className = 'book';
                bookDiv.innerHTML = `
                                    <img src="${book.coverImage ? `data:image/jpeg;base64,${book.coverImage}` : 'placeholder.jpg'}" alt="${book.bookTitle} Cover" />
                                    <div>
                                        <h3>${book.bookTitle}</h3>
                                        <p><strong>Pages:</strong> ${book.bookPages}</p>
                                        <p><strong>Summary:</strong> ${book.bookSummary}</p>
                                    </div>
                                `;
                bookListDiv.appendChild(bookDiv);
            });
        })
        .catch(error => {
            console.error('Error fetching books:', error);
            bookListDiv.innerHTML = '<p>Error loading books.</p>'; // Show error message
        });
}