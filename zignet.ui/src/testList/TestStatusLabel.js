import React, { Component } from 'react';
import TestStatusBadge from './TestStatusBadge'
import TestStatusLabelNew from './TestStatusLabelNew'

class TestStatusLabel extends Component {

  render() {
    return (
      <span>
        <TestStatusBadge failingFromDate={this.props.failingFromDate}/>
        <TestStatusLabelNew failingFromDate={this.props.failingFromDate} passingFromDate={this.props.passingFromDate}/>
      </span>
    );
  }
}

export default TestStatusLabel;
