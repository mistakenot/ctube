var thumbnail = function(id) {
    return "https://img.youtube.com/vi/" + id + "/3.jpg";
}

var VM = function() {
    var self = this;
    self.currentVideo = ko.observable({id: "none"});
    self.nextVideos = ko.observableArray([]);
    self.topics = ko.observable([]);
    self.iframeUrl = ko.computed(() => "https://www.youtube.com/embed/" + self.currentVideo().id + "?autoplay=1&origin=http://example.com");

    self.toggleSubject = (topic) => {
        var topics = self.topics();
        var topic = topics.filter(s => s.label == topic.label)[0];

        if (topic) {
            topic.isActive = !topic.isActive;
        }

        self.topics([]);
        self.topics(topics)
        self.load();
    }

    self.clickOnNextVideo = (video) => {
        self.currentVideo(video);
    }

    self.load = () => {
        var topics = self.topics().filter(t => t.isActive).map(t => t.label);
        var query = topics.reduce((s, v, i) => s + "topics=" + v + "&", "");

        $.getJSON(
            "Home/Load?" + query,
            (data, status, xhr) => {
                console.info(data);
                self.nextVideos(data.suggestions);

                if (data.suggestions.length > 0) {
                    self.currentVideo(data.suggestions[0]);
                }
            })
            .error((xhr, status, error) => {
                console.error(error);
            })
    }

    self.init = () => {
        $.getJSON(
            "Home/Topics",
            (data, status, xhr) => {
                var topics = data.map(l => ({label: l, isActive: true}));
                console.info(data);
                self.topics(topics);
                self.load();
            })
            .error((xhr, status, error) => {
                console.error(error);
            })
    }
}

var vm = new VM();

ko.applyBindings(vm);
vm.init();
