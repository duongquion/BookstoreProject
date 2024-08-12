var swiper = new Swiper(".mySwiper", {
    pagination: {
        el: ".swiper-pagination",
      },
    
      navigation: {
        nextEl: ".ri-arrow-drop-right-line",
        prevEl: ".ri-arrow-drop-left-line",
      },
});
// Check if there's a stored target date in localStorage
var storedTargetDate = localStorage.getItem('targetDate');

// If there's no stored target date, set a new one for 24 hours from now
if (!storedTargetDate) {
    var targetDate = new Date().getTime() + (24 * 60 * 60 * 1000); // 24 hours from now
    localStorage.setItem('targetDate', targetDate);
} else {
    var targetDate = parseInt(storedTargetDate, 10);
}

// Update the count down every 1 second
var countdownFunction = setInterval(function() {

    // Get today's date and time
    var now = new Date().getTime();

    // Find the distance between now and the target date
    var distance = targetDate - now;

    // Time calculations for hours, minutes and seconds
    var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = Math.floor((distance % (1000 * 60)) / 1000);

    // Display the result in the respective elements
    document.getElementById("hours").innerHTML = hours < 10 ? "0" + hours : hours;
    document.getElementById("minutes").innerHTML = minutes < 10 ? "0" + minutes : minutes;
    document.getElementById("seconds").innerHTML = seconds < 10 ? "0" + seconds : seconds;

    // If the count down is finished, reset the target date for another 24 hours
    if (distance < 0) {
        clearInterval(countdownFunction);
        var newTargetDate = new Date().getTime() + (24 * 60 * 60 * 1000); // Another 24 hours from now
        localStorage.setItem('targetDate', newTargetDate);
        location.reload(); // Refresh the page to reset the countdown display
    }
}, 1000);

