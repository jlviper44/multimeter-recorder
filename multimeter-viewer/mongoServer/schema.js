const mongoose = require("mongoose");

// Schema for Users
const Data = mongoose.Schema(
  {
    Date:
    {
      type: Date,
      required: true,
    },
    Value:
    {
      type: Number,
      required: true,
    }
  },
);
const dataModel = mongoose.model("data", Data);

module.exports = {
  DataModel: dataModel,
};
