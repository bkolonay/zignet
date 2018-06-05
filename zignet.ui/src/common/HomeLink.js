import React, { Component } from 'react';

class HomeLink extends Component {

  render() {
    const link = this.props.grouped ? 
      <a href="/?group=true">ZigNet</a> :
      <a href="/">ZigNet</a>;

    return (
      link
    );
  }
}

export default HomeLink;
