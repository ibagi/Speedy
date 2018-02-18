const { ipcRenderer } = require("electron");

const getBarWidth = (value, min = 0, max = 300, scaleMin = 1, scaleMax = 100000) => {
    if (!value || value <= 0)
        return '50px';

    let minv = Math.log(scaleMin);
    let maxv = Math.log(scaleMax);
    let scale = (maxv - minv) / (max - min);
    return ((Math.log(value) - minv) / scale + min) + 'px';
}

const getBarCaption = (value) => {
    if (!value || value <= 0)
        return { value: 0, caption: 'KB/s' };

    if (value < 1024)
        return { value, caption: 'KB/s' };

    return { value: (value / 1024).toFixed(2), caption: 'MB/s' };
}

const SpeedData = ({ name, download, upload, downloadWidth, uploadWidth }) => `
    <div class="container">
        <div>
            ${name}
        </div>
        <div class="bar" style="width: ${downloadWidth};">
            ${download.value} ${download.caption}
        </div>
        <div class="bar" style="width: ${uploadWidth};">
            ${upload.value} ${upload.caption}
        </div>
    </div>`;

const SpeedDataList = (items) =>
    items.map((i) => SpeedData({
        name: i.interfaceName,
        download: getBarCaption(i.downloadInKiloBit),
        upload: getBarCaption(i.uploadInKiloBit),
        downloadWidth: getBarWidth(i.downloadInKiloBit),
        uploadWidth: getBarWidth(i.uploadInKiloBit)
    })).join('');

class App {
    constructor(el) {
        this.$el = el;

        ipcRenderer.on('res-refresh', (event, data) => {
            this.render(data);
        });

        window.setInterval(() => {
            ipcRenderer.send('req-refresh');
        }, 500);
    }

    render(items) {
        this.$el.innerHTML = SpeedDataList(items);
    }
}

window.addEventListener('DOMContentLoaded', () => {
    let mount = document.getElementById('app');
    const app = new App(mount || document.body);
});