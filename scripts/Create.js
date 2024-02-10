var stars = [];
var traps = [];
var tempPlat = [];
var sprites = [];
var mapType = "blank";
var maxCredits = 20;
var creditsLabel;
var Credits = maxCredits;
var platforms;
var cost;
const self = this;
var TB1 = document.getElementById("TB1");
var TB2 = document.getElementById("TB2");
var TB3 = document.getElementById("TB3");
var TB4 = document.getElementById("TB4");
var TB5 = document.getElementById("TB5");
var bg;

class MapCreator extends Phaser.Scene {
    constructor() {
        super();
    }
    preload() {
        this.load.image('bg', 'assets/skies/space3stretch.png');
        this.load.image('bg2', 'assets/pics/platformer-backdrop.png');
        this.load.image('street', 'assets/sprites/cyberpunk-street.png');
        //this.load.image('bg3', 'assets/pics/backscroller.png');
        this.load.spritesheet('saw', 'assets/sprites/saws.png', { frameWidth: 548, frameHeight: 548 });
        this.load.image('star', 'assets/sprites/star.png');
        this.load.image('bluePortal', 'assets/sprites/portal.png');
        this.load.image('ground1', 'assets/sets/objects/platform1.png');
        this.load.image('ground2', 'assets/sets/objects/platform2.png');
        this.load.image('ground3', 'assets/sets/objects/platform3.png');
        this.load.image('ground4', 'assets/sprites/platformBlock.png');
        this.load.spritesheet('portalEmpty', 'assets/sprites/portalRings1.png', { frameWidth: 32, frameHeight: 32 });
        this.load.spritesheet('portalFull', 'assets/sprites/portalRings2.png', { frameWidth: 32, frameHeight: 32 });
        this.load.spritesheet('dude', 'assets/sprites/dude.png', { frameWidth: 32, frameHeight: 48 });
    }
    create() {
        platforms = this.add.group();
        var flag1 = false;
        var flag2 = true;
        bg = this.add.image(375, 300, 'bg2').setScale(2.34);
        creditsLabel = this.add.text(16, 16, 'Credits left: ' + maxCredits, { fontSize: '14px', fill: '#000' });
        platforms.create(150, 250, 'ground2');
        platforms.create(600, 460, 'ground3');
        platforms.create(600, 175, 'ground2');
        platforms.create(1000, 175, 'ground2');
        platforms.create(300, 400, 'ground1')
        platforms.create(0, 600, 'ground4');
        platforms.create(500, 600, 'ground4');
        const emptyPortal = this.add.sprite(1180, 50, 'portalEmpty').setScale(2);
        const fullPortal = this.add.sprite(1260, 50, 'portalFull').setScale(2);
        const saw = this.add.sprite(1225, 125, 'saw').setScale(0.15);
        const star = this.add.sprite(1225, 200, 'star').setScale(0.5);
        var sp;
        sprites = [saw, star, emptyPortal, fullPortal];
        sprites.forEach(setter);
        function setter(sprite) {
            sprite.setInteractive({ draggable: true });
        }
        this.input.on('drag', (pointer, gameObject, dragX, dragY) => {
            switch (gameObject.texture.key) {
                case "saw": cost = 5; break;
                default: cost = 0; break;
            }
            if (gameObject.x < 1150 && dragX < 1150) cost = 0;
            if (Credits >= cost) {
                dragX = Phaser.Math.Clamp(dragX, 0, 1250);
                gameObject.x = dragX;
                dragY = Phaser.Math.Clamp(dragY, -100, 600);
                gameObject.y = dragY;
                flag1 = true;
            }
            else if (flag2) {
                alert('Not enough credits!');
                flag2 = false;
            }
        });
        this.input.on('dragend', (pointer, gameObject, dragX, dragY) => {
            if (flag1) {
                if (gameObject.x > 1150) {
                    addCredits(cost);
                    gameObject.destroy();
                    if (gameObject.texture.key === "portalEmpty") emptyPortal = this.add.sprite(1180, 50, 'portalEmpty').setScale(2); emptyPortal.setInteractive({ draggable: true }); traps.push(emptyPortal);
                    if (gameObject.texture.key === "portalFull") fullPortal = this.add.sprite(1260, 50, 'portalFull').setScale(2); fullPortal.setInteractive({ draggable: true }); traps.push(fullPortal);
                }
                else {
                    var oldY = 0;
                    var oldX = 1225;
                    var spriteScale = 1;
                    switch (gameObject.texture.key) {
                        case "saw": oldY = 125; spriteScale = 0.15; addCredits(-5); break;
                        case "star": oldY = 200; spriteScale = 0.5; break;
                        case "portalEmpty": oldX = 1175; oldY = 50; spriteScale = 2; break;
                        case "portalFull": oldX = 1250; oldY = 50; spriteScale = 2; break;
                    }
                    if (gameObject.texture.key != "portalEmpty" && gameObject.texture.key != "portalFull")
                        sp = this.add.sprite(oldX, oldY, gameObject.texture).setScale(spriteScale);
                    if (gameObject.texture.key === "star") stars.push(gameObject);
                    else traps.push(gameObject);
                    sp.setInteractive({ draggable: true });
                }
                flag1 = false;
            }
        });
        function addCredits(sum) {
            Credits += sum;
            creditsLabel.setText("Credits left: " + Credits)
        }
    }
    changeMapType() {
        mapType = "classic";
        function destroyer(sprite) {
            sprite.body.destroy();
        }
        console.log('log!');
        console.log(self.platforms);
        platforms.clear(true, true);
        platforms.forEach(destroyer);
        console.log(self.platforms);
        //stars.forEach(destroyer);
        traps.forEach(destroyer);
        switch (mapType) {
            case "blank":
                maxCredits = 20;
                Credits = maxCredits;
                platforms.create(150, 250, 'ground2');
                platforms.create(600, 460, 'ground3');
                platforms.create(600, 175, 'ground2');
                platforms.create(1000, 175, 'ground2');
                platforms.create(300, 400, 'ground1')
                platforms.create(0, 600, 'ground4');
                platforms.create(500, 600, 'ground4');
                break;
            case "classic":
                maxCredits = 25;
                Credits = maxCredits;
                bg.destroy();
                bg = this.add.image(375, 300, 'bg').setScale(2.34);
                // ...
                break;
        }
    }
}
const mapCreatorInstance = new MapCreator();
document.getElementById('MapTypesDDL').addEventListener('change', function () {
    mapCreatorInstance.changeMapType();
});
function saveMap() {
    creditsLabel.setText("");
    TB1.value = ""; TB2.value = ""; TB3.value = ""; TB4.value = ""; TB5.value = "";
    mergeArr(stars, TB1, TB2, null);
    mergeArr(traps, TB3, TB4, TB5);
}
function mergeArr(arr, TBa, TBb, TBc) {
    for (let i = 0; i < arr.length; i++) {
        TBa.value += arr[i].x + "_";
        TBb.value += arr[i].y + "_";
        if (TBc != null) {
            TBc.value += arr[i].texture.key + "_";
        }
    }
    TBa.value = TBa.value.substring(0, TBa.value.length - 1);
    TBb.value = TBb.value.substring(0, TBb.value.length - 1);
}

function confirmDiscard() {
    if (confirm("Are you sure? This map will be erased."))
        window.location.href = 'Explore.aspx';
}

const config = {
    type: Phaser.AUTO,
    width: 1300,
    height: 600,
    parent: 'phaser-example',
    scene: MapCreator
};
const game = new Phaser.Game(config);

//Check if both portals were chosen (dragged in)
//Add a "Test Map" button that temporarily saves the map, moves to Play.aspx with a special code and deletes it afterwards.