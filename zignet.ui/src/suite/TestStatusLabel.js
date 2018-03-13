import React, { Component } from 'react';

class TestStatusLabel extends Component {

  _getLabelText() {
    if (this.props.failingFromDate)
      return "Down " + this.props.failingFromDate;
    else
      return "Up " + this.props.passingFromDate;
  }

  _getClassName() {
    return this.props.failingFromDate ? "text-danger" : "text-success";
  }

  render() {
    return (
      <span className={this._getClassName()}>{this._getLabelText()}</span>
    );
  }
}

export default TestStatusLabel;
