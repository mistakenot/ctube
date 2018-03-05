var VM = function() {
    var self = this;
    self.currentVideo = ko.observable("testing");
    self.nextVideos = ko.observableArray([]);
    self.topics = ko.observable([]);

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

    }

    self.load = () => {
        var topics = self.topics().filter(t => t.isActive).map(t => t.label);
        var query = topics.reduce((s, v, i) => s + "topics=" + v + "&", "");

        $.getJSON(
            "Home/Load?" + query,
            (data, status, xhr) => {
                console.info(data);
                self.nextVideos(data.suggestions);
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
