const express = require("express");
const bodyParser = require("body-parser");
const mongoose = require("mongoose");
const cors = require("cors");
const app = express();

app.use(cors());
app.use(express.json({ limit: "50mb" }));
app.use(express.urlencoded({ limit: "50mb" }));
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

mongoose.connect("mongodb://elrond:27017/MultimeterOutput", {
  useNewUrlParser: true,
  useUnifiedTopology: true,
  useFindAndModify: false,
});

var db = mongoose.connection;
db.on("open", () => {
  console.log("Connected to mongoDB");
});
db.on("error", (error) => {
  console.log(error);
});

const { DataModel } = require("./schema");

function getCollections(req, res)
{
  try {
    mongoose.connection.db.listCollections().toArray( (err, names) => {
      if(err){
        res.send("Error while fetching Databases");
      }
      else{
        var arr = [];
        names.forEach( (item) => {
          arr.push(item.name);
        });
        res.send(arr);
      }
    });
  }
  catch
  {
    res.send("Error while fetching Databases");
  }
}

function getItems(req, res)
{
  try
  {
    var name = req.params.colName;
    mongoose.connection.db.collection(name, (err, collection) => {
      if (err)
        res.send("Error while fetching Items");
      else
      {
        if(req.params.type == "count")
          collection.countDocuments().then((count) => {
            res.send(count.toString());
            // console.log(data)
          })
          // res.send();
        else if(req.params.type == "all")
          collection.find({}).toArray((err, data) => {
            if (err)
              res.send("Error while fetching Items");
            else
            {
              res.send(data);
            }
          });
      }
    });
  }
  catch
  {
    res.send("Error while fetching Items");
  }
}

function getItemsLimited(req, res)
{
  try
  {
    var name = req.params.colName;
    mongoose.connection.db.collection(name, (err, collection) => {
      if (err)
        res.send("Error while fetching Items");
      else
      {
        collection.aggregate([{$match:{}}, { $sort : { Date:-1 } }, {$limit: 100}]).toArray((err, data) => {
          if (err)
            res.send("Error while fetching Items");
          else
          {
            res.send(data);
          }
        });
      }
    });
  }
  catch
  {
    res.send("Error while fetching Items");
  }
}

// connection routes
app.get("/getCollections", (req, res) => {
  // var clientIP = requestIp.getClientIp(req);
  // res.send(clientIP);
  getCollections(req, res);
});

app.get("/collection/:colName", (req, res) => {
  // getConfig(req, res);
  // getItems(req, res);
    getItemsLimited(req, res);
});
app.get("/collection/:colName/:type", (req, res) => {
  // getConfig(req, res);
  // getItems(req, res);
    getItems(req, res);
});

app.listen(3001, () => {
  console.log("Server started on port 3001");
});
