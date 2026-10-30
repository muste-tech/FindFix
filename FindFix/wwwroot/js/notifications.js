function enableJobNotifications(){
    if (!("Notification" in window)) {
        alert("This browser does not support notifications.");
        return;
    }
    Notification.requestPermission().then (function(permission){
        if (permission == "granted") {
            localStorage.setItem("findfixNotifications", "on");
            new Notification ("FindFix", {body:"Job notifications are now on."});
        }
    });
}
(function () {
    const BASELINE_KEY = "findfixjobstatuses";

    function notify(text){
        new Notification("FindFix", {body: text});
    }
    function checkjobs(){
        if (localStorage.getItem("findfixNotifications") !== "on") return;
        if(!("Notification" in window) || Notification.permission !== "granted") return;

        fetch("/Notification/JobStatuses", {credentials: "same-origin"})
        .then(function (r) {return r.ok ? r.json():null;})
        .then(function(data){
            if(!data) return;

            const saved = JSON.parse(localStorage.getItem(BASELINE_KEY) || "{}");
            const previous = saved[data.owner];

            // store the current statuses for the user
            const current = {};
            data.jobs.forEach(function(j){ current[j.id] = j.status; });
            saved[data.owner] = current;
            localStorage.setItem(BASELINE_KEY, JSON.stringify(saved));

            // First check for the user record only, don't notify
            if(previous === undefined) return;
            data.jobs.forEach(function (j){
                const old = previous[j.id];
                if (old === undefined){
                    if (j.status ==="pending") notify("You have a new job request.");
                }else if(old !==j.status){
                    notify("Job # " + j.id + " is now " + j.status + ".");
                }
            });
        })
        .catch(function (){/*not logged in or offline: do nothing */});
    }
    checkjobs();
    setInterval(checkjobs, 20000);
})();